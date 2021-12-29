-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- ホスト: 127.0.0.1
-- 生成日時: 2021-12-28 10:47:35
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
-- データベース: `priconner_user`
--

-- --------------------------------------------------------

--
-- テーブルの構造 `userdata`
--

CREATE TABLE `userdata` (
  `id` bigint(20) NOT NULL,
  `name` varchar(30) CHARACTER SET utf8 NOT NULL,
  `rank` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `lastlogin` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `created` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- テーブルのデータのダンプ `userdata`
--

INSERT INTO `userdata` (`id`, `name`, `rank`, `exp`, `lastlogin`, `created`) VALUES
(1, 'test_user', 1, 0, '2021-12-04 09:11:24', '2021-12-04 09:11:24'),
(2, '沙羅曼蛇', 1, 0, '2021-12-04 08:52:16', '2021-12-04 08:52:16'),
(3, 'テスト', 1, 0, '2021-12-04 08:52:48', '2021-12-04 08:52:48'),
(4, 'TEST', 1, 0, '2021-12-04 09:03:23', '2021-12-04 09:03:23'),
(5, 'uh', 1, 0, '2021-12-08 03:06:43', '2021-12-08 03:06:43'),
(6, 'asadsf', 1, 0, '2021-12-08 05:41:22', '2021-12-08 05:41:22'),
(7, 'dd', 1, 0, '2021-12-08 05:43:16', '2021-12-08 05:43:16'),
(8, 'asfas', 1, 0, '2021-12-08 07:02:42', '2021-12-08 07:02:42'),
(9, 'aaa', 1, 0, '2021-12-08 07:03:24', '2021-12-08 07:03:24'),
(10, 'aaa', 1, 0, '2021-12-08 07:07:18', '2021-12-08 07:07:18'),
(11, 'kjhkj', 1, 0, '2021-12-08 07:10:25', '2021-12-08 07:10:25'),
(12, 'ddkdk', 1, 0, '2021-12-08 07:11:22', '2021-12-08 07:11:22'),
(13, 'wand', 1, 0, '2021-12-12 00:38:38', '2021-12-12 00:38:38'),
(14, 'kjk', 1, 0, '2021-12-12 00:40:37', '2021-12-12 00:40:37'),
(15, 'genji', 1, 0, '2021-12-12 00:41:09', '2021-12-12 00:41:09'),
(16, 'send', 1, 0, '2021-12-12 00:42:46', '2021-12-12 00:42:46'),
(17, 'SSS', 1, 0, '2021-12-12 00:51:12', '2021-12-12 00:51:12'),
(18, 'FDFD', 1, 0, '2021-12-12 08:15:44', '2021-12-12 08:15:44'),
(19, 'aaBCD', 1, 0, '2021-12-13 08:06:55', '2021-12-13 08:06:55'),
(20, 'CCC', 1, 0, '2021-12-13 08:07:42', '2021-12-13 08:07:42'),
(21, 'abc', 1, 0, '2021-12-14 22:48:40', '2021-12-14 22:48:40'),
(22, 'asfd', 1, 0, '2021-12-15 05:50:15', '2021-12-15 05:50:15'),
(23, '32', 1, 0, '2021-12-15 05:50:49', '2021-12-15 05:50:49'),
(24, 'asd', 1, 0, '2021-12-15 05:54:15', '2021-12-15 05:54:15'),
(25, 'んｖｈ', 1, 0, '2021-12-15 05:55:21', '2021-12-15 05:55:21'),
(26, 'hj', 1, 0, '2021-12-15 06:09:59', '2021-12-15 06:09:59'),
(27, 'あｓｄ', 1, 0, '2021-12-15 06:10:25', '2021-12-15 06:10:25'),
(28, 'jf', 1, 0, '2021-12-15 21:03:01', '2021-12-15 21:03:01'),
(29, 'hgj', 1, 0, '2021-12-15 22:11:33', '2021-12-15 22:11:33'),
(30, '111', 1, 0, '2021-12-16 03:43:29', '2021-12-16 03:43:29'),
(31, '111', 1, 0, '2021-12-16 04:49:06', '2021-12-16 04:49:06'),
(32, 'asfd', 1, 0, '2021-12-16 04:54:15', '2021-12-16 04:54:15'),
(33, 'aaa', 1, 0, '2021-12-16 06:19:01', '2021-12-16 06:19:01'),
(34, 'aa', 1, 0, '2021-12-16 06:22:41', '2021-12-16 06:22:41'),
(35, 'AVC', 1, 0, '2021-12-16 22:09:27', '2021-12-16 22:09:27'),
(36, 'avx', 1, 0, '2021-12-16 22:32:58', '2021-12-16 22:32:58'),
(37, '111', 1, 0, '2021-12-16 22:36:33', '2021-12-16 22:36:33'),
(38, 'kj', 1, 0, '2021-12-16 23:22:18', '2021-12-16 23:22:18'),
(39, '123', 1, 0, '2021-12-16 23:24:52', '2021-12-16 23:24:52'),
(40, 'fghhft', 1, 0, '2021-12-17 00:10:44', '2021-12-17 00:10:44'),
(41, 'SASAyama', 1, 0, '2021-12-17 00:11:35', '2021-12-17 00:11:35'),
(42, 'SANDA', 1, 0, '2021-12-17 05:19:07', '2021-12-17 05:19:07'),
(43, 'SAOYAMA', 1, 0, '2021-12-19 21:42:18', '2021-12-19 21:42:18'),
(44, 'AAAS', 1, 0, '2021-12-21 03:09:55', '2021-12-21 03:09:55'),
(45, 'YAMADA', 1, 0, '2021-12-27 15:33:33', '2021-12-27 15:33:33'),
(46, 'サメ', 1, 0, '2021-12-27 15:38:44', '2021-12-27 15:38:44'),
(47, 'Shark', 1, 0, '2021-12-27 15:41:25', '2021-12-27 15:41:25'),
(48, 'Shark', 1, 0, '2021-12-27 15:42:51', '2021-12-27 15:42:51'),
(49, 'Shark', 1, 0, '2021-12-27 17:23:51', '2021-12-27 17:23:51'),
(50, 'Shark', 1, 0, '2021-12-27 17:37:18', '2021-12-27 17:37:18'),
(51, 'サメ', 1, 0, '2021-12-27 17:45:48', '2021-12-27 17:45:48');

-- --------------------------------------------------------

--
-- テーブルの構造 `useritem`
--

CREATE TABLE `useritem` (
  `id` int(11) NOT NULL,
  `user_id` bigint(20) NOT NULL,
  `item_id` int(11) NOT NULL,
  `num` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- テーブルのデータのダンプ `useritem`
--

INSERT INTO `useritem` (`id`, `user_id`, `item_id`, `num`) VALUES
(1, 2, 1, 1),
(2, 2, 2, 2);

-- --------------------------------------------------------

--
-- テーブルの構造 `userunit`
--

CREATE TABLE `userunit` (
  `id` int(11) NOT NULL,
  `user_id` bigint(20) NOT NULL,
  `unit_id` int(11) NOT NULL,
  `exp` int(11) NOT NULL,
  `level` int(11) NOT NULL,
  `equipment1` int(11) NOT NULL,
  `equipment2` int(11) NOT NULL,
  `equipment3` int(11) NOT NULL,
  `equipment4` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- テーブルのデータのダンプ `userunit`
--

INSERT INTO `userunit` (`id`, `user_id`, `unit_id`, `exp`, `level`, `equipment1`, `equipment2`, `equipment3`, `equipment4`) VALUES
(1, 1, 1, 0, 1, 0, 0, 0, 0);

--
-- ダンプしたテーブルのインデックス
--

--
-- テーブルのインデックス `userdata`
--
ALTER TABLE `userdata`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `useritem`
--
ALTER TABLE `useritem`
  ADD PRIMARY KEY (`id`);

--
-- テーブルのインデックス `userunit`
--
ALTER TABLE `userunit`
  ADD PRIMARY KEY (`id`);

--
-- ダンプしたテーブルの AUTO_INCREMENT
--

--
-- テーブルの AUTO_INCREMENT `userdata`
--
ALTER TABLE `userdata`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=52;

--
-- テーブルの AUTO_INCREMENT `useritem`
--
ALTER TABLE `useritem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- テーブルの AUTO_INCREMENT `userunit`
--
ALTER TABLE `userunit`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
