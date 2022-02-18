create database dbFinalEmarketing

use dbFinalEmarketing

create table tbl_Admins(
a_id int primary key identity,
a_username nvarchar(50) unique not null,
a_email nvarchar(50) not null,
a_password nvarchar(50) not null,
a_image nvarchar(max) default 'admin.png'
)

insert into tbl_Admins(a_username,a_email,a_password) values('Mubeen','Mubeen@gmail.com','Mubeen123')
insert into tbl_Admins(a_username,a_email,a_password) values('Numair','Numair@gmail.com','Numair123')
insert into tbl_Admins(a_username,a_email,a_password) values('Ashna','Ashna@gmail.com','Ashna123')
insert into tbl_Admins(a_username,a_email,a_password) values('Admin','Admin@gmail.com','Admin123')
insert into tbl_Admins(a_username,a_email,a_password) values('Root','Root@gmail.com','Admin123')
select * from tbl_Admins


create table tbl_Categories(
cat_id int primary key identity,
cat_name nvarchar(50) not null,
cat_image nvarchar(max) not null,
)

select  * from tbl_Categories

create table tbl_Images(
i_id int primary key identity,
i_image nvarchar(max),
p_id int foreign key references tbl_Products(pro_id)
)

create table tbl_Users(
u_id int primary key identity,
u_username nvarchar(50) unique not null,
u_email nvarchar(50) not null,
a_password nvarchar(50) not null,
a_gender nvarchar(50) not null,
a_phone nvarchar(50) not null,
a_country nvarchar(50) not null,
u_image nvarchar(max)
)

create table tbl_Products(
pro_id int primary key identity,
pro_name nvarchar(50) unique not null,
pro_price nvarchar(50) not null,
pro_description nvarchar(50) not null,
pro_fk_user int foreign key references tbl_Users(u_id),
pro_fk_cat int foreign key references tbl_Categories(cat_id),
pro_status int not null,
pro_add_date date not null
)

select * from tbl_Products
select * from tbl_Categories
select * from tbl_Users
select * from tbl_Images
