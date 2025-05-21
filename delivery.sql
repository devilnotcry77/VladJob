CREATE DATABASE DeliveryDB;
USE DeliveryDB;

CREATE TABLE Couriers (
    courier_id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    courier_name VARCHAR(255) NOT NULL,
    courier_phone VARCHAR(20) NOT NULL
);

CREATE TABLE Vehicles (
    vehicle_id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    courier_id INT UNSIGNED,
    license_plate VARCHAR(20) NOT NULL,
    model VARCHAR(255) NOT NULL,
    can_carry_large BOOLEAN NOT NULL,
    FOREIGN KEY (courier_id) REFERENCES Couriers(courier_id)
);

CREATE TABLE Orders (
    order_id INT UNSIGNED AUTO_INCREMENT PRIMARY KEY,
    order_date DATE NOT NULL,
    customer_name VARCHAR(255) NOT NULL,
    customer_address VARCHAR(255) NOT NULL,
    customer_phone VARCHAR(20) NOT NULL,
    status ENUM('в обработке', 'доставлен') NOT NULL,
    product_type ENUM('крупногабаритный', 'мелкогабаритный') NOT NULL,
    planned_delivery_date DATE NOT NULL,
    planned_delivery_time TIME NOT NULL,
    courier_id INT UNSIGNED,
    actual_delivery_date DATE,
    actual_delivery_time TIME,
    FOREIGN KEY (courier_id) REFERENCES Couriers(courier_id)
);