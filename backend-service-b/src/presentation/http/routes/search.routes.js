const express = require('express');
const { SearchController } = require('../controllers/Controllers');

const router = express.Router();

router.get('/', SearchController.search);

module.exports = router;
