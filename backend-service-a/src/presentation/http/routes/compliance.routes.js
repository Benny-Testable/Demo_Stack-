const express = require('express');
const { ComplianceController } = require('../controllers/ComplianceController');

const router = express.Router();

router.get('/privacy', ComplianceController.getPrivacyPolicy);
router.get('/gdpr/export/:userId', ComplianceController.exportUserData);
router.delete('/gdpr/erase/:userId', ComplianceController.eraseUserData);
router.post('/coppa/consent', ComplianceController.handleCoppaConsent);
router.delete('/ferpa/student/:studentId', ComplianceController.deleteStudentRecord);
router.get('/soc2/audit-logs', ComplianceController.getAuditLogs);
router.post('/pci/mask-card', ComplianceController.maskCardData);

module.exports = router;
