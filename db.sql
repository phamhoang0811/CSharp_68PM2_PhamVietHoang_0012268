
CREATE DATABASE QLSinhVienCSharp;
GO

USE QLSinhVienCSharp;
GO
IF OBJECT_ID('Students', 'U') IS NOT NULL
    DROP TABLE Students;

IF OBJECT_ID('Classes', 'U') IS NOT NULL
    DROP TABLE Classes;

CREATE TABLE Classes (
    ClassId VARCHAR(50) NOT NULL PRIMARY KEY,
    ClassName NVARCHAR(100) NOT NULL,
    Note NVARCHAR(255)
);
GO

ALTER TABLE Classes
ADD ClassCode VARCHAR(50);

CREATE TABLE Students (
    MSSV VARCHAR(50) NOT NULL PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE,
    Gender NVARCHAR(10),
    ClassId VARCHAR(50) NOT NULL,
    CONSTRAINT FK_Students_Classes
        FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);
GO
DELETE FROM Students;
DELETE FROM Classes;

INSERT INTO Classes (ClassId, ClassName)
VALUES
('68PM1', N'Lớp 68PM1'),
('68PM2', N'Lớp 68PM2'),
('68PM3', N'Lớp 68PM3');

-- Thêm 10 sinh viên lớp 68PM1
INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
VALUES
('SV01', N'Nguyễn Văn An', '2004-01-01', N'Nam', '68PM1'),
('SV02', N'Trần Thị Bình', '2004-01-02', N'Nữ', '68PM1'),
('SV03', N'Lê Hoàng Minh', '2004-01-03', N'Nam', '68PM1'),
('SV04', N'Phạm Thu Trang', '2004-01-04', N'Nữ', '68PM1'),
('SV05', N'Đỗ Quang Huy', '2004-01-05', N'Nam', '68PM1'),
('SV06', N'Vũ Ngọc Mai', '2004-01-06', N'Nữ', '68PM1'),
('SV07', N'Hoàng Đức Anh', '2004-01-07', N'Nam', '68PM1'),
('SV08', N'Ngô Minh Châu', '2004-01-08', N'Nữ', '68PM1'),
('SV09', N'Bùi Quốc Bảo', '2004-01-09', N'Nam', '68PM1'),
('SV10', N'Đặng Thị Lan', '2004-01-10', N'Nữ', '68PM1');

-- Thêm 10 sinh viên lớp 68PM2
INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
VALUES
('SV11', N'Nguyễn Hoàng Long', '2004-02-01', N'Nam', '68PM2'),
('SV12', N'Trần Thu Hà', '2004-02-02', N'Nữ', '68PM2'),
('SV13', N'Lý Công Thành', '2004-02-03', N'Nam', '68PM2'),
('SV14', N'Phan Ngọc Ánh', '2004-02-04', N'Nữ', '68PM2'),
('SV15', N'Võ Thanh Tùng', '2004-02-05', N'Nam', '68PM2'),
('SV16', N'Đinh Mỹ Linh', '2004-02-06', N'Nữ', '68PM2'),
('SV17', N'Nguyễn Minh Quân', '2004-02-07', N'Nam', '68PM2'),
('SV18', N'Tạ Hồng Nhung', '2004-02-08', N'Nữ', '68PM2'),
('SV19', N'Lương Gia Bảo', '2004-02-09', N'Nam', '68PM2'),
('SV20', N'Chu Thị Hạnh', '2004-02-10', N'Nữ', '68PM2');

-- Thêm 10 sinh viên lớp 68PM3
INSERT INTO Students (MSSV, FullName, DateOfBirth, Gender, ClassId)
VALUES
('SV21', N'Nguyễn Quốc Khánh', '2004-03-01', N'Nam', '68PM3'),
('SV22', N'Trịnh Thu Phương', '2004-03-02', N'Nữ', '68PM3'),
('SV23', N'Đỗ Minh Hiếu', '2004-03-03', N'Nam', '68PM3'),
('SV24', N'Phạm Bảo Ngọc', '2004-03-04', N'Nữ', '68PM3'),
('SV25', N'Hoàng Văn Nam', '2004-03-05', N'Nam', '68PM3'),
('SV26', N'Nguyễn Thị Hoa', '2004-03-06', N'Nữ', '68PM3'),
('SV27', N'Bùi Đức Mạnh', '2004-03-07', N'Nam', '68PM3'),
('SV28', N'Lê Thị Thu', '2004-03-08', N'Nữ', '68PM3'),
('SV29', N'Phan Quốc Việt', '2004-03-09', N'Nam', '68PM3'),
('SV30', N'Vũ Thanh Hằng', '2004-03-10', N'Nữ', '68PM3');

-- Kiểm tra lại


SELECT * FROM Classes;
SELECT * FROM Students;