/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 80036
Source Host           : localhost:3306
Source Database       : stock_review

Target Server Type    : MYSQL
Target Server Version : 80036
File Encoding         : 65001

Date: 2024-06-19 11:48:12
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for tb_bulletinboard
-- ----------------------------
DROP TABLE IF EXISTS `tb_bulletinboard`;
CREATE TABLE `tb_bulletinboard` (
  `Id` bigint NOT NULL COMMENT '主键',
  `TodayLimitUp` varchar(50) DEFAULT NULL COMMENT '今日涨停',
  `YesterdayLimitUp` varchar(50) DEFAULT NULL COMMENT '昨日涨停',
  `TodayLimitDown` varchar(50) DEFAULT NULL COMMENT '今日下跌',
  `YesterdayLimitDown` varchar(50) DEFAULT NULL COMMENT '昨日下跌',
  `TodayRise` varchar(50) DEFAULT NULL COMMENT '今天上涨',
  `YesterdayRise` varchar(50) DEFAULT NULL COMMENT '昨天上涨',
  `TodayFall` varchar(50) DEFAULT NULL COMMENT '今天下跌',
  `YesterdayFall` varchar(50) DEFAULT NULL COMMENT '昨天下跌',
  `TodayCalorimeter` varchar(50) DEFAULT NULL COMMENT '今日能量',
  `YesterdayCalorimeter` varchar(50) DEFAULT NULL COMMENT '昨日能量',
  `NorthboundFunds` varchar(50) DEFAULT NULL COMMENT '北向资金',
  `SecondBoardPercent` varchar(50) DEFAULT NULL COMMENT '二板',
  `ThirdBoardPercent` varchar(50) DEFAULT NULL COMMENT '三板',
  `HighBoardPercent` varchar(50) DEFAULT NULL COMMENT '高板',
  `EmotionPercent` varchar(50) DEFAULT NULL COMMENT '情绪值',
  `TodayFryingRate` varchar(50) DEFAULT NULL COMMENT '今天炸板',
  `YesterdayFryingRate` varchar(50) DEFAULT NULL COMMENT '昨天炸板',
  `CityPower` varchar(50) DEFAULT NULL COMMENT '城市能量',
  `TodayZTPBRate` varchar(50) DEFAULT NULL COMMENT '今日涨停破板率',
  `YesterdayZTJBX` varchar(50) DEFAULT NULL COMMENT '昨日涨停今表现',
  `YesterdayLBJBX` varchar(50) DEFAULT NULL COMMENT '昨日连扳今表现',
  `YesterdayPBJBX` varchar(50) DEFAULT NULL COMMENT '昨日破板今表现',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `UpdateTime` datetime NOT NULL COMMENT '修改时间',
  `Status` tinyint(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='股票看板';

-- ----------------------------
-- Records of tb_bulletinboard
-- ----------------------------
