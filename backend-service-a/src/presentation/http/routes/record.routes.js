const express = require('express');
const { RecordController } = require('../controllers/RecordController');

const router = express.Router();

router.get('/', RecordController.listRecords);
router.get('/:id', RecordController.getRecord);
router.post('/', RecordController.createRecord);

module.exports = router;
