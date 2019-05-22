create table FoodType
(
	type_id int identity
		primary key,
	type_name varchar(50)
)
go

create table OrderStatus
(
	status_id int identity
		primary key,
	status_name varchar(20) not null
)
go

create table Restaurant
(
	rest_id int identity
		primary key,
	name varchar(100) not null,
	street_name varchar(50) not null,
	street_number varchar(29) not null,
	postal_code smallint not null,
	city varchar(50) not null
)
go

create table RestaurantFoodTypes
(
	rest_id int not null
		references Restaurant,
	type_id int not null
		references FoodType,
	primary key (rest_id, type_id)
)
go

create table UsersRole
(
	role_id int identity
		primary key,
	role_name varchar(20) not null
)
go

create table Users
(
	user_id int identity
		primary key,
	firstname varchar(50) not null,
	lastname varchar(50) not null,
	email varchar(100) not null,
	password varchar(100) not null,
	role_id int not null
		references UsersRole
)
go

create table CustomerInfo
(
	street_name varchar(50) not null,
	street_number varchar(20) not null,
	postal_code smallint not null,
	city varchar(50) not null,
	user_id int not null
		references Users
)
go

create table Orders
(
	order_id int identity
		primary key,
	user_id int not null
		references Users,
	rest_id int not null
		references Restaurant,
	status_id int not null
		references OrderStatus
)
go

create table Staff
(
	user_id int not null
		references Users,
	rest_id int not null
		references Restaurant,
	primary key (user_id, rest_id)
)
go
