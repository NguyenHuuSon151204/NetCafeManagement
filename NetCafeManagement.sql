-- Tạo database
CREATE DATABASE NetCafeManagement;
GO

USE NetCafeManagement;
GO

-- Bảng ACCOUNTS
CREATE TABLE ACCOUNTS (
    AccountID INT PRIMARY KEY IDENTITY(1,1),
    Username VARCHAR(50) UNIQUE NOT NULL,
    PasswordHash VARCHAR(100) NOT NULL,
    Salt VARCHAR(50) NOT NULL,
    IsActive BIT DEFAULT 1,
    LastLogin DATETIME,
    FailedAttempts INT DEFAULT 0
);

-- Bảng EMPLOYEES
CREATE TABLE EMPLOYEES (
    EmployeeID INT PRIMARY KEY IDENTITY(1,1),
    AccountID INT UNIQUE FOREIGN KEY REFERENCES ACCOUNTS(AccountID),
    FullName NVARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    Role VARCHAR(10) CHECK (Role IN ('Admin', 'Manager', 'Staff', 'Technician')),
    IsActive BIT DEFAULT 1
);

-- Bảng SHIFTS
CREATE TABLE SHIFTS (
    ShiftID INT PRIMARY KEY IDENTITY(1,1),
    EmployeeID INT FOREIGN KEY REFERENCES EMPLOYEES(EmployeeID),
    StartTime DATETIME NOT NULL DEFAULT GETDATE(),
    EndTime DATETIME,
    StartCash DECIMAL(15,2) NOT NULL,
    EndCash DECIMAL(15,2),
    Notes NVARCHAR(500)
);

-- Bảng COMPUTERS
CREATE TABLE COMPUTERS (
    ComputerID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(50) NOT NULL,
    Tier VARCHAR(10) CHECK (Tier IN ('Basic', 'Duo', 'Premium')),
    HourlyRate DECIMAL(10,2) NOT NULL,
    Status TINYINT DEFAULT 0, -- 0: Off, 1: Available, 2: Busy, 3: Maintenance
    CONSTRAINT CHK_Status CHECK (Status BETWEEN 0 AND 3)
);

-- Bảng CUSTOMERS
CREATE TABLE CUSTOMERS (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Phone VARCHAR(20),
    Balance DECIMAL(15,2) DEFAULT 0,
    JoinDate DATETIME DEFAULT GETDATE()
);

-- Bảng SESSIONS
CREATE TABLE SESSIONS (
    SessionID INT PRIMARY KEY IDENTITY(1,1),
    ComputerID INT FOREIGN KEY REFERENCES COMPUTERS(ComputerID),
    CustomerID INT NULL FOREIGN KEY REFERENCES CUSTOMERS(CustomerID),
    StartTime DATETIME NOT NULL DEFAULT GETDATE(),
    EndTime DATETIME,
    TotalAmount DECIMAL(15,2)
);

-- Bảng PRODUCTS
CREATE TABLE PRODUCTS (
    ProductID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Category VARCHAR(20) CHECK (Category IN ('Food', 'Drink', 'Combo')),
    Price DECIMAL(10,2) NOT NULL,
    IsActive BIT DEFAULT 1
);

-- Bảng ORDERS
CREATE TABLE ORDERS (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    SessionID INT NULL FOREIGN KEY REFERENCES SESSIONS(SessionID),
    ShiftID INT NULL FOREIGN KEY REFERENCES SHIFTS(ShiftID),
    OrderTime DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(15,2)
);

-- Bảng ORDER_DETAILS
CREATE TABLE ORDER_DETAILS (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES ORDERS(OrderID),
    ProductID INT FOREIGN KEY REFERENCES PRODUCTS(ProductID),
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(10,2) NOT NULL
);

-- Bảng TRANSACTIONS
CREATE TABLE TRANSACTIONS (
    TransactionID INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT FOREIGN KEY REFERENCES CUSTOMERS(CustomerID),
    Amount DECIMAL(15,2) NOT NULL,
    Type VARCHAR(20) CHECK (Type IN ('Deposit', 'Withdraw', 'Refund')),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE PROCEDURE sp_CalculateSessionTotal
    @SessionID INT
AS
BEGIN
    DECLARE @ComputerCharge DECIMAL(15,2);
    DECLARE @FoodCharge DECIMAL(15,2);
    
    -- Tính tiền máy
    SELECT @ComputerCharge = 
        DATEDIFF(MINUTE, StartTime, ISNULL(EndTime, GETDATE())) / 60.0 * c.HourlyRate
    FROM SESSIONS s
    JOIN COMPUTERS c ON s.ComputerID = c.ComputerID
    WHERE s.SessionID = @SessionID;
    
    -- Tính tiền đồ ăn
    SELECT @FoodCharge = ISNULL(SUM(od.Quantity * od.UnitPrice), 0)
    FROM ORDER_DETAILS od
    JOIN ORDERS o ON od.OrderID = o.OrderID
    WHERE o.SessionID = @SessionID;
    
    -- Cập nhật tổng tiền
    UPDATE SESSIONS
    SET TotalAmount = @ComputerCharge + @FoodCharge
    WHERE SessionID = @SessionID;
    
    SELECT @ComputerCharge AS ComputerCharge, @FoodCharge AS FoodCharge;
END

CREATE PROCEDURE sp_EndShift
    @ShiftID INT,
    @EndCash DECIMAL(15,2)
AS
BEGIN
    DECLARE @ComputerRevenue DECIMAL(15,2);
    DECLARE @FoodRevenue DECIMAL(15,2);
    
    -- Tính doanh thu máy
    SELECT @ComputerRevenue = ISNULL(SUM(s.TotalAmount), 0)
    FROM SESSIONS s
    WHERE s.EndTime BETWEEN 
        (SELECT StartTime FROM SHIFTS WHERE ShiftID = @ShiftID) 
        AND GETDATE();
    
    -- Tính doanh thu đồ ăn
    SELECT @FoodRevenue = ISNULL(SUM(o.TotalAmount), 0)
    FROM ORDERS o
    WHERE o.ShiftID = @ShiftID;
    
    -- Cập nhật ca làm việc
    UPDATE SHIFTS
    SET 
        EndTime = GETDATE(),
        EndCash = @EndCash,
        Notes = CONCAT('Doanh thu máy: ', @ComputerRevenue, ' | Doanh thu đồ ăn: ', @FoodRevenue)
    WHERE ShiftID = @ShiftID;
    
    SELECT @ComputerRevenue AS ComputerRevenue, @FoodRevenue AS FoodRevenue;
END

-- Tài khoản admin
INSERT INTO ACCOUNTS (Username, PasswordHash, Salt) 
VALUES ('admin', 'admin', 'salted123'); -- password: admin

-- Tài khoản nhân viên
INSERT INTO ACCOUNTS (Username, PasswordHash, Salt) VALUES 
('staff1', '123123', 'salt123'), -- password: 123456
('staff2', '123123', 'salt123'),
('tech1', '123123', 'salt123');

-- Thông tin nhân viên
INSERT INTO EMPLOYEES (AccountID, FullName, Phone, Role) VALUES
(1, 'Nguyễn Văn Admin', '0987654321', 'Admin'),
(2, 'Trần Thị Thu Ngân', '0912345678', 'Staff'),
(3, 'Phạm Văn Nhân Viên', '0978123456', 'Staff'),
(4, 'Lê Văn Kỹ Thuật', '0967891234', 'Technician');

INSERT INTO COMPUTERS (Name, Tier, HourlyRate, Status) VALUES
-- Máy Basic
('Máy 1', 'Basic', 10000, 1),
('Máy 2', 'Basic', 10000, 1),
('Máy 3', 'Basic', 10000, 0), -- Đang tắt
-- Máy Duo
('Máy đôi 1', 'Duo', 15000, 1),
('Máy đôi 2', 'Duo', 15000, 2), -- Đang sử dụng
-- Máy Premium
('Máy VIP 1', 'Premium', 20000, 1),
('Máy VIP 2', 'Premium', 20000, 3); -- Đang bảo trì

INSERT INTO CUSTOMERS (Name, Phone, Balance, JoinDate) VALUES
('Khách vãng lai', NULL, 0, GETDATE()),
('Nguyễn Văn A', '0912345678', 500000, '2023-01-15'),
('Trần Thị B', '0987654321', 200000, '2023-02-20'),
('Lê Văn C', '0978123456', 0, GETDATE());

INSERT INTO PRODUCTS (Name, Category, Price) VALUES
-- Thức uống
('Coca Cola', 'Drink', 15000),
('Pepsi', 'Drink', 15000),
('Sting', 'Drink', 12000),
('Cà phê đen', 'Drink', 10000),
-- Đồ ăn
('Mì tôm', 'Food', 20000),
('Bánh mì', 'Food', 15000),
('Xúc xích', 'Food', 10000),
-- Combo
('Combo 1: 1 giờ + nước', 'Combo', 50000),
('Combo 2: 2 giờ + mì', 'Combo', 80000);

-- Ca đang hoạt động
INSERT INTO SHIFTS (EmployeeID, StartTime, StartCash) VALUES
(2, DATEADD(HOUR, -4, GETDATE()), 5000000);

-- Ca đã kết thúc
INSERT INTO SHIFTS (EmployeeID, StartTime, EndTime, StartCash, EndCash, Notes) VALUES
(3, '2023-05-01 08:00:00', '2023-05-01 16:00:00', 5000000, 7500000, 'Ca sáng 1/5');

-- Phiên đang hoạt động
INSERT INTO SESSIONS (ComputerID, CustomerID, StartTime) VALUES
(5, 2, DATEADD(MINUTE, -30, GETDATE())); -- Khách đang dùng máy đôi 2

-- Phiên đã kết thúc
INSERT INTO SESSIONS (ComputerID, CustomerID, StartTime, EndTime, TotalAmount) VALUES
(1, 1, '2023-05-01 10:00:00', '2023-05-01 12:30:00', 25000), -- 2.5 giờ x 10,000
(4, 3, '2023-05-01 14:00:00', '2023-05-01 16:00:00', 30000); -- 2 giờ x 15,000

-- Đơn hàng gắn với phiên
INSERT INTO ORDERS (SessionID, OrderTime, TotalAmount) VALUES
(1, '2023-05-01 11:00:00', 45000);

INSERT INTO ORDER_DETAILS (OrderID, ProductID, Quantity, UnitPrice) VALUES
(1, 1, 2, 15000), -- 2 Coca
(1, 5, 1, 20000); -- 1 Mì tôm

-- Đơn hàng lẻ (không gắn phiên)
INSERT INTO ORDERS (ShiftID, OrderTime, TotalAmount) VALUES
(1, DATEADD(HOUR, -2, GETDATE()), 30000);

INSERT INTO ORDER_DETAILS (OrderID, ProductID, Quantity, UnitPrice) VALUES
(2, 3, 2, 12000), -- 2 Sting
(2, 7, 1, 10000); -- 1 Xúc xích

INSERT INTO TRANSACTIONS (CustomerID, Amount, Type) VALUES
(2, 500000, 'Deposit'), -- Nạp tiền
(3, 200000, 'Deposit'),
(2, -100000, 'Withdraw'), -- Trừ tiền khi dùng máy
(3, -50000, 'Withdraw');

-- Xem danh sách máy tính và trạng thái
SELECT ComputerID, Name, Tier, 
       CASE Status 
           WHEN 0 THEN 'Tắt' 
           WHEN 1 THEN 'Sẵn sàng' 
           WHEN 2 THEN 'Đang dùng' 
           ELSE 'Bảo trì' 
       END AS StatusText
FROM COMPUTERS;

-- Xem doanh thu theo ca
SELECT s.ShiftID, e.FullName, s.StartTime, s.EndTime, 
       s.EndCash - s.StartCash AS Revenue,
       s.Notes
FROM SHIFTS s
JOIN EMPLOYEES e ON s.EmployeeID = e.EmployeeID;

-- Xem lịch sử giao dịch của khách
SELECT c.Name, t.Amount, t.Type, t.CreatedAt
FROM TRANSACTIONS t
JOIN CUSTOMERS c ON t.CustomerID = c.CustomerID
ORDER BY t.CreatedAt DESC;

SELECT * FROM Computers
