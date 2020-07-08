/*
 Navicat Premium Data Transfer

 Source Server         : MariaDB
 Source Server Type    : MariaDB
 Source Server Version : 100412
 Source Host           : localhost:3306
 Source Schema         : stagedb

 Target Server Type    : MariaDB
 Target Server Version : 100412
 File Encoding         : 65001

 Date: 28/03/2020 15:40:48
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for accounts
-- ----------------------------
DROP TABLE IF EXISTS `accounts`;
CREATE TABLE `accounts`  (
  `nID` int(11) NOT NULL AUTO_INCREMENT,
  `sEmail` varchar(50) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `sPassword` varchar(32) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  `nRole` int(11) NULL DEFAULT NULL,
  `dCreateTime` datetime(0) NOT NULL,
  `sCreateIP` varchar(15) CHARACTER SET latin1 COLLATE latin1_swedish_ci NOT NULL,
  PRIMARY KEY (`nID`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 2 CHARACTER SET = latin1 COLLATE = latin1_swedish_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of accounts
-- ----------------------------
INSERT INTO `accounts` VALUES (1, 'user@example.com', 'string', 4, '2020-03-28 14:42:47', '127.0.0.1');

-- ----------------------------
-- Procedure structure for p_User_Create
-- ----------------------------
DROP PROCEDURE IF EXISTS `p_User_Create`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `p_User_Create`(IN email varchar(50), OUT ret int)
BEGIN
	IF EXISTS(SELECT 1 FROM users WHERE sEmail = email) THEN
	BEGIN
		SET ret = 1;
	END;
	ELSE
	BEGIN
		SET ret = 2;
	END;
	END IF;
END
;;
delimiter ;

-- ----------------------------
-- Procedure structure for p_User_Login
-- ----------------------------
DROP PROCEDURE IF EXISTS `p_User_Login`;
delimiter ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `p_User_Login`(IN `sEmail` varchar(50), IN `sPassword` varchar(32), OUT `nRet` int)
BEGIN
	IF EXISTS(SELECT 1 FROM accounts WHERE accounts.sEmail = sEmail AND accounts.sPassword = sPassword) THEN
	BEGIN
		SELECT * FROM accounts WHERE accounts.sEmail = sEmail AND accounts.sPassword = sPassword;
		SET nRet = 0;
	END;
	ELSE
	BEGIN
		SET nRet = 1;
	END;
	END IF;
END
;;
delimiter ;

SET FOREIGN_KEY_CHECKS = 1;
