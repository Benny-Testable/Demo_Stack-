function securityHeadersMiddleware(req, res, next) {
  res.setHeader('X-Content-Type-Options', 'nosniff');
  res.setHeader('X-Frame-Options', 'DENY');
  res.setHeader('X-XSS-Protection', '1; mode=block');
  res.setHeader('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');
  res.setHeader('Content-Security-Policy', "default-src 'self'");
  next();
}

function errorHandlerMiddleware(err, req, res, next) {
  console.error('[service-a][presentation-error]', err.message);
  res.status(err.status || 500).json({
    error: 'Internal Server Error',
    message: process.env.NODE_ENV === 'test' ? err.message : 'An unexpected error occurred',
    code: err.code || 'SERVER_ERROR',
  });
}

module.exports = {
  securityHeadersMiddleware,
  errorHandlerMiddleware,
};
