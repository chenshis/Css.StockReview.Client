/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 80036
Source Host           : localhost:3306
Source Database       : stock_review

Target Server Type    : MYSQL
Target Server Version : 80036
File Encoding         : 65001

Date: 2024-06-19 11:48:23
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tb_timeindexchart
-- ----------------------------
DROP TABLE IF EXISTS `tb_timeindexchart`;
CREATE TABLE `tb_timeindexchart` (
  `Id` bigint NOT NULL COMMENT '主键',
  `StockId` varchar(50) NOT NULL COMMENT '股票代码',
  `StockName` varchar(50) NOT NULL COMMENT '股票名称',
  `Time` varchar(50) NOT NULL COMMENT '时间',
  `Latest` varchar(50) NOT NULL COMMENT '最新',
  `Avg` varchar(50) NOT NULL COMMENT '平均',
  `Turnover` varchar(50) NOT NULL COMMENT '成交量',
  `Volume` varchar(50) NOT NULL COMMENT '成交额',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL COMMENT '修改时间',
  `Status` tinyint(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='股票分时图';

-- ----------------------------
-- Records of tb_timeindexchart
-- ----------------------------
