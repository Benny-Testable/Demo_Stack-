/**
 * Security Headers & Error Handling Middleware
 * - Enforces HSTS, CSP, X-Frame-Options, X-Content-Type-Options
 * - Prevents Information Disclosure / Stack Traces in HTTP 500 responses
 */

function securityHeadersMiddleware(req, res, next) {
  res.setHeader('X-Content-Type-Options', 'nosniff');
  res.setHeader('X-Frame-Options', 'DENY');
  res.setHeader('X-XSS-Protection', '1; mode=block');
  res.setHeader('Strict-Transport-Security', 'max-age=31536000; includeSubDomains');
  res.setHeader('Content-Security-Policy', "default-src 'self'");
  next();
}

function errorHandlerMiddleware(err, req, res, next) {
  // Never leak internal stack traces to clients (OWASP / SOC 2)
  console.error('[service-a][error]', err.message);
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
