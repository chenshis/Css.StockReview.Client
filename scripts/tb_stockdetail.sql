/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 80036
Source Host           : localhost:3306
Source Database       : stock_review

Target Server Type    : MYSQL
Target Server Version : 80036
File Encoding         : 65001

Date: 2024-06-20 14:59:54
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tb_stockdetail
-- ----------------------------
DROP TABLE IF EXISTS `tb_stockdetail`;
CREATE TABLE `tb_stockdetail` (
  `Id` bigint NOT NULL COMMENT '主键',
  `StockId` varchar(50) NOT NULL COMMENT '股票代码',
  `StockName` varchar(50) NOT NULL COMMENT '股票名称',
  `ZF` varchar(50) DEFAULT NULL COMMENT '涨幅',
  `Latest` varchar(50) DEFAULT NULL COMMENT '最新',
  `Reason` varchar(50) DEFAULT NULL COMMENT '原因',
  `FirstLetterTime` varchar(50) DEFAULT NULL COMMENT '首封时间',
  `LastFBan` varchar(50) DEFAULT NULL COMMENT '最后封板',
  `LimitUpType` varchar(50) DEFAULT NULL COMMENT '涨停形态',
  `OpenNum` varchar(50) DEFAULT NULL COMMENT '开板',
  `SeveralDaysBan` varchar(50) DEFAULT NULL COMMENT '几天几板',
  `FDanMoney` varchar(50) DEFAULT NULL COMMENT '封单额',
  `TurnoverRate` varchar(50) DEFAULT NULL COMMENT '换手率',
  `CurrencyMoney` varchar(50) DEFAULT NULL COMMENT '流通值亿',
  `LBanNum` varchar(50) DEFAULT NULL COMMENT '连板',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL COMMENT '修改时间',
  `Status` tinyint(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='股票明细';
