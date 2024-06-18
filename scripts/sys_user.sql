/*
Navicat MySQL Data Transfer

Source Server         : 127.0.0.1
Source Server Version : 80036
Source Host           : localhost:3306
Source Database       : stock_review

Target Server Type    : MYSQL
Target Server Version : 80036
File Encoding         : 65001

Date: 2024-06-18 19:30:38
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for sys_user
-- ----------------------------
DROP TABLE IF EXISTS `sys_user`;
CREATE TABLE `sys_user` (
  `Id` bigint NOT NULL COMMENT '主键',
  `UserName` varchar(50) NOT NULL COMMENT '用户名',
  `Password` varchar(255) NOT NULL COMMENT '密码',
  `Contacts` varchar(50) NOT NULL COMMENT '联系人',
  `Phone` varchar(50) NOT NULL COMMENT '手机号码',
  `QQ` varchar(50) NOT NULL COMMENT 'QQ',
  `Role` int NOT NULL COMMENT '角色;1，2，4，8 免费用户 普通用户 vip 管理员',
  `CreateUser` varchar(50) DEFAULT NULL COMMENT '创建人',
  `CreateTime` datetime NOT NULL COMMENT '创建时间',
  `UpdateUser` varchar(50) DEFAULT NULL COMMENT '修改人',
  `UpdateTime` datetime NOT NULL COMMENT '修改时间',
  `Status` tinyint(1) NOT NULL COMMENT '是否删除',
  `Jti` varchar(50) NOT NULL COMMENT 'jwt id标识',
  `Expires` datetime DEFAULT NULL COMMENT '过期时间',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='系统用户表';

-- ----------------------------
-- Records of sys_user
-- ----------------------------
INSERT INTO `sys_user` VALUES ('6983525277546016', 'admin', '2e237e1c75a51285bcd5bbf9d5d5631a', '管理员', '111111', '1234', '8', 'admin', '2024-05-07 18:15:46', 'admin', '2024-05-12 10:39:51', '0', 'f5852d322d514f22b51f0e3f68e417dd', '2040-12-31 00:00:00');
