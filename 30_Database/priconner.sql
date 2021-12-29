-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- ホスト: 127.0.0.1
-- 生成日時: 2021-12-28 10:47:56
-- サーバのバージョン： 10.4.17-MariaDB
-- PHP のバージョン: 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- データベース: `priconner`
--

-- --------------------------------------------------------

--
-- テーブルの構造 `aura`
--

CREATE TABLE `aura` (
  `id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `type` int(11) NOT NULL,
  `affect_type` int(11) NOT NULL,
  `value` int(11) NOT NULL,
  `affect_time` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `aura`
--

INSERT INTO `aura` (`id`, `name`, `type`, `affect_type`, `value`, `affect_time`) VALUES
(1, '攻10', 1, 1, 10, 30),
(2, '防10', 1, 2, 10, 30);

-- --------------------------------------------------------

--
-- テーブルの構造 `enemy`
--

CREATE TABLE `enemy` (
  `id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `type` int(11) NOT NULL DEFAULT 1,
  `boss` int(11) NOT NULL DEFAULT 0,
  `asset` varchar(30) NOT NULL,
  `skill` int(11) NOT NULL DEFAULT 0,
  `behaviour` int(11) NOT NULL DEFAULT 0,
  `health` int(11) NOT NULL DEFAULT 10,
  `strength` int(11) NOT NULL DEFAULT 10,
  `def` int(11) NOT NULL DEFAULT 1,
  `pow` int(11) NOT NULL DEFAULT 1,
  `dex` int(11) NOT NULL DEFAULT 1,
  `intelligent` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `enemy`
--

INSERT INTO `enemy` (`id`, `name`, `type`, `boss`, `asset`, `skill`, `behaviour`, `health`, `strength`, `def`, `pow`, `dex`, `intelligent`) VALUES
(1, 'one', 1, 0, '', 0, 0, 10, 10, 1, 1, 1, 1),
(2, 'two', 1, 0, '', 0, 0, 10, 10, 1, 1, 1, 1);

-- --------------------------------------------------------

--
-- テーブルの構造 `equipment`
--

CREATE TABLE `equipment` (
  `id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL DEFAULT 'NO_NAME',
  `main_category` int(6) NOT NULL DEFAULT 1,
  `sub_category` int(6) NOT NULL DEFAULT 1,
  `part` int(11) NOT NULL DEFAULT 1,
  `value` int(11) NOT NULL DEFAULT 0,
  `rate` int(11) NOT NULL DEFAULT 1,
  `attribute` int(11) NOT NULL DEFAULT 0,
  `secial_id` int(11) NOT NULL DEFAULT 0,
  `price` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `equipment`
--

INSERT INTO `equipment` (`id`, `name`, `main_category`, `sub_category`, `part`, `value`, `rate`, `attribute`, `secial_id`, `price`) VALUES
(1, '木剣', 1, 1, 1, 0, 1, 0, 0, 0),
(2, 'マウス', 1, 1, 1, 0, 1, 0, 0, 0);

-- --------------------------------------------------------

--
-- テーブルの構造 `item`
--

CREATE TABLE `item` (
  `Id` int(11) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `Description` varchar(30) NOT NULL,
  `Type` int(11) NOT NULL DEFAULT 1 COMMENT 'タイプID',
  `Value` int(11) NOT NULL DEFAULT 1 COMMENT '効力',
  `Rate` int(11) NOT NULL DEFAULT 1,
  `Time` int(11) NOT NULL DEFAULT 0 COMMENT 'ミリ秒',
  `DotRate` int(11) NOT NULL DEFAULT 0,
  `Price` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `item`
--

INSERT INTO `item` (`Id`, `Name`, `Description`, `Type`, `Value`, `Rate`, `Time`, `DotRate`, `Price`) VALUES
(1, 'ポーション', '体力を10回復する', 1, 10, 1, 0, 0, 30),
(2, 'ハイポーション', '100回復', 1, 100, 1, 0, 0, 0),
(3, 'パワポ', 'パワー10回復', 2, 10, 1, 0, 0, 0),
(4, 'ハイパワポ', 'パワー100回復', 2, 100, 1, 0, 0, 0);

-- --------------------------------------------------------

--
-- テーブルの構造 `jobskill`
--

CREATE TABLE `jobskill` (
  `id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `job` int(11) NOT NULL,
  `must_level` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `jobskill`
--

INSERT INTO `jobskill` (`id`, `name`, `job`, `must_level`) VALUES
(1, 'ダミー', 1, 2),
(2, 'ダミー2', 1, 3);

-- --------------------------------------------------------

--
-- テーブルの構造 `level`
--

CREATE TABLE `level` (
  `id` int(11) NOT NULL,
  `unit_id` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `health` int(11) NOT NULL,
  `strength` int(11) NOT NULL,
  `def` int(11) NOT NULL,
  `pow` int(11) NOT NULL,
  `dex` int(11) NOT NULL,
  `intelligent` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `level`
--

INSERT INTO `level` (`id`, `unit_id`, `level`, `exp`, `health`, `strength`, `def`, `pow`, `dex`, `intelligent`) VALUES
(1, 1, 1, 0, 11, 11, 11, 11, 11, 11),
(2, 1, 2, 30, 12, 12, 12, 12, 12, 12),
(3, 1, 3, 60, 13, 13, 13, 13, 13, 13);

-- --------------------------------------------------------

--
-- テーブルの構造 `quest`
--

CREATE TABLE `quest` (
  `id` int(11) NOT NULL,
  `name` varchar(30) NOT NULL,
  `reward_id` int(11) NOT NULL,
  `class` int(11) NOT NULL,
  `exp` int(11) NOT NULL DEFAULT 1,
  `map_id` int(11) NOT NULL,
  `scenario_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `quest`
--

INSERT INTO `quest` (`id`, `name`, `reward_id`, `class`, `exp`, `map_id`, `scenario_id`) VALUES
(1, '初心者の館', 1, 1, 10, 1, 1),
(2, 'ころころ', 1, 1, 11, 1, 2);

-- --------------------------------------------------------

--
-- テーブルの構造 `rank`
--

CREATE TABLE `rank` (
  `id` int(11) NOT NULL,
  `rank` int(11) NOT NULL,
  `exp` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `rank`
--

INSERT INTO `rank` (`id`, `rank`, `exp`) VALUES
(1, 1, 0),
(2, 2, 30),
(3, 3, 60),
(4, 4, 99),
(5, 5, 150);

-- --------------------------------------------------------

--
-- テーブルの構造 `unit`
--

CREATE TABLE `unit` (
  `Id` int(11) NOT NULL,
  `Name` varchar(30) NOT NULL,
  `LevelId` int(11) NOT NULL,
  `SkillId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `Job` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- テーブルのデータのダンプ `unit`
--

INSERT INTO `unit` (`Id`, `Name`, `LevelId`, `SkillId`, `Type`, `Job`) VALUES
(1, 'ゆにちゃんず', 1, 1, 1, 1);

--
-- ダンプしたテーブルのインデックス
--

--
-- テーブルのインデックス `aura`
--
ALTER TABLE `aura`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `enemy`
--
ALTER TABLE `enemy`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `equipment`
--
ALTER TABLE `equipment`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `item`
--
ALTER TABLE `item`
  ADD PRIMARY KEY (`Id`);

--
-- テーブルのインデックス `jobskill`
--
ALTER TABLE `jobskill`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `level`
--
ALTER TABLE `level`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `quest`
--
ALTER TABLE `quest`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `rank`
--
ALTER TABLE `rank`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `unit`
--
ALTER TABLE `unit`
  ADD PRIMARY KEY (`Id`);

--
-- ダンプしたテーブルの AUTO_INCREMENT
--

--
-- テーブルの AUTO_INCREMENT `aura`
--
ALTER TABLE `aura`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- テーブルの AUTO_INCREMENT `enemy`
--
ALTER TABLE `enemy`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- テーブルの AUTO_INCREMENT `equipment`
--
ALTER TABLE `equipment`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- テーブルの AUTO_INCREMENT `item`
--
ALTER TABLE `item`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- テーブルの AUTO_INCREMENT `jobskill`
--
ALTER TABLE `jobskill`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- テーブルの AUTO_INCREMENT `level`
--
ALTER TABLE `level`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- テーブルの AUTO_INCREMENT `quest`
--
ALTER TABLE `quest`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- テーブルの AUTO_INCREMENT `rank`
--
ALTER TABLE `rank`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- テーブルの AUTO_INCREMENT `unit`
--
ALTER TABLE `unit`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
