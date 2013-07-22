CREATE DATABASE Supermarket;

USE Supermarket;
-- MySQL dump 10.13  Distrib 5.6.12, for Win64 (x86_64)
--
-- Host: localhost    Database: supermarket
-- ------------------------------------------------------
-- Server version	5.6.12

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `measures`
--

DROP TABLE IF EXISTS `measures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `measures` (
  `MeasureId` int(11) NOT NULL AUTO_INCREMENT,
  `MeasureName` varchar(45) NOT NULL,
  PRIMARY KEY (`MeasureId`)
) ENGINE=InnoDB AUTO_INCREMENT=105 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `measures`
--

LOCK TABLES `measures` WRITE;
/*!40000 ALTER TABLE `measures` DISABLE KEYS */;
INSERT INTO `measures` VALUES (100,'litres'),(101,'pieces'),(102,'kilograms'),(103,'pakages'),(104,'grams');
/*!40000 ALTER TABLE `measures` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `products`
--

DROP TABLE IF EXISTS `products`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `products` (
  `ProductId` int(11) NOT NULL AUTO_INCREMENT,
  `VendorId` int(11) NOT NULL,
  `ProductName` varchar(50) NOT NULL,
  `MeasureId` int(11) NOT NULL,
  `BasePrice` decimal(10,2) NOT NULL,
  PRIMARY KEY (`ProductId`),
  KEY `fk_Products_Measures_idx` (`MeasureId`),
  KEY `fk_Products_Vendors1_idx` (`VendorId`),
  CONSTRAINT `fk_Products_Measures` FOREIGN KEY (`MeasureId`) REFERENCES `measures` (`MeasureId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_Products_Vendors1` FOREIGN KEY (`VendorId`) REFERENCES `vendors` (`VendorId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=52 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `products`
--

LOCK TABLES `products` WRITE;
/*!40000 ALTER TABLE `products` DISABLE KEYS */;
INSERT INTO `products` VALUES (1,1,'BMW X5',101,5000.00),(2,1,'BMW X6',101,6000.00),(3,1,'BMW X8',101,8000.00),(4,2,'ASTRA',101,3000.00),(5,3,'NIVA',101,2000.00),(6,3,'SAMARA',101,1000.00),(7,4,'Galardo',101,100000.00),(8,4,'Cabrio',101,200000.00),(9,5,'Gold',100,2.00),(10,5,'Dark',100,2.00),(11,6,'Dark',100,1.00),(12,7,'Ariana',100,2.00),(13,7,'Fresh',100,1.00),(14,8,'Light',100,1.50),(15,8,'Normal',100,1.00),(16,8,'Zero',100,2.00),(17,8,'Fata',100,1.20),(18,9,'Cola',100,1.00),(19,9,'Mirinda',100,1.00),(20,10,'apple',100,0.50),(21,10,'peach',100,0.70),(22,10,'gropes',100,1.00),(23,10,'orange',100,1.00),(24,10,'mandarine',100,1.00),(25,11,'minds',103,2.00),(26,11,'drops',103,1.00),(27,11,'profesional',103,3.00),(28,12,'sticks',103,1.00),(29,12,'chocolate',103,1.00),(30,12,'max',103,2.00),(31,14,'sweets',103,2.00),(32,14,'chocolate',103,3.00),(33,14,'cookies',103,2.00),(34,14,'badems',103,2.00),(35,15,'chocolate',101,2.00),(36,15,'white choco',101,2.00),(37,16,'chocolate',101,2.00),(38,16,'white choco',101,1.00),(39,17,'nuts',103,1.00),(40,17,'badems',103,1.00),(41,17,'frize',103,2.00),(42,18,'sushi',104,12.00),(43,18,'sushi ocean',104,14.00),(44,19,'apple',100,3.00),(45,19,'gropes',100,3.00),(46,19,'peach',100,3.00),(47,19,'pine-apple',100,5.00),(48,20,'energy',100,3.00),(49,21,'energy',100,4.00),(50,22,'energy',100,3.00),(51,22,'max energy',100,5.00);
/*!40000 ALTER TABLE `products` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `vendors`
--

DROP TABLE IF EXISTS `vendors`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `vendors` (
  `VendorId` int(11) NOT NULL AUTO_INCREMENT,
  `VendorName` varchar(50) NOT NULL,
  PRIMARY KEY (`VendorId`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `vendors`
--

LOCK TABLES `vendors` WRITE;
/*!40000 ALTER TABLE `vendors` DISABLE KEYS */;
INSERT INTO `vendors` VALUES (1,'BMW'),(2,'OPEL'),(3,'LADA'),(4,'LAMBORGHINI'),(5,'Zagorka Corp.'),(6,'Kamenitsa Corp.'),(7,'Ariana Corp.'),(8,'Coca-Cola'),(9,'Pepsy'),(10,'Derby'),(11,'Orbit'),(12,'Moreni'),(13,'Nura'),(14,'Nestle'),(15,'MIlka'),(16,'Svoge'),(17,'Chipi'),(18,'Sushi'),(19,'Fresh BG'),(20,'Moster'),(21,'SHark'),(22,'Redbull');
/*!40000 ALTER TABLE `vendors` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2013-07-22 20:41:44
