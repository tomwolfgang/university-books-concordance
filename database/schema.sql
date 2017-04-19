CREATE TABLE `word` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `value` varchar(100) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `length` int(11) DEFAULT NULL,
  PRIMARY KEY (`value`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `document` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `gutenberg_id` varchar(255) NOT NULL,
  `title` varchar(2000) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `author` varchar(200) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  `local_filename` varchar(255) DEFAULT NULL,
  `release_date` date DEFAULT NULL,
  `load_state` int(11) NOT NULL,
  PRIMARY KEY (`gutenberg_id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `contains` (
  `doc_id` int(11) NOT NULL,
  `word_id` int(11) NOT NULL,
  `line` int(11) NOT NULL,
  `file_offset` int(11) NOT NULL,
  `sentence` int(11) NOT NULL,
  `index_in_sentence` int(11) NOT NULL,
  `paragraph` int(11) NOT NULL,
  `page` int(11) NOT NULL,
  KEY `word_id_idx` (`word_id`),
  KEY `doc_id_idx` (`doc_id`),
  CONSTRAINT `doc_id` FOREIGN KEY (`doc_id`) REFERENCES `document` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `word_id` FOREIGN KEY (`word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `group` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`name`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `groups_words` (
  `group_id` int(11) NOT NULL,
  `word_id` int(11) NOT NULL,
  KEY `word_id_idx` (`word_id`),
  KEY `group_id_idx` (`group_id`),
  CONSTRAINT `gid` FOREIGN KEY (`group_id`) REFERENCES `group` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `wid` FOREIGN KEY (`word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `relation` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8 COLLATE utf8_bin NOT NULL,
  PRIMARY KEY (`name`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `relations_words` (
  `relation_id` int(11) NOT NULL,
  `first_word_id` int(11) NOT NULL,
  `second_word_id` int(11) NOT NULL,
  PRIMARY KEY (`relation_id`,`first_word_id`,`second_word_id`),
  KEY `first_word_id_idx` (`first_word_id`),
  KEY `second_word_id_idx` (`second_word_id`),
  KEY `relation_id_idx` (`relation_id`),
  CONSTRAINT `fwid` FOREIGN KEY (`first_word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `rid` FOREIGN KEY (`relation_id`) REFERENCES `relation` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `swid` FOREIGN KEY (`second_word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `phrase` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=0 DEFAULT CHARSET=utf8 COLLATE=utf8_bin;

CREATE TABLE `phrases_words` (
  `phrase_id` int(11) NOT NULL,
  `word_id` int(11) NOT NULL,
  `ordinal` int(11) NOT NULL,
  PRIMARY KEY (`phrase_id`,`word_id`,`ordinal`),
  KEY `ordinal_idx` (`ordinal`),
  KEY `word_id_idx` (`word_id`),
  KEY `phrase_id_idx` (`phrase_id`),
  CONSTRAINT `pwid` FOREIGN KEY (`word_id`) REFERENCES `word` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `pid` FOREIGN KEY (`phrase_id`) REFERENCES `phrase` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
