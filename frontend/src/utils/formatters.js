const LOCALE = 'en-US';

export function formatCurrency(value) {
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '-';
  }
  return new Intl.NumberFormat(LOCALE, {
    style: 'currency',
    currency: 'USD',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2,
  }).format(Number(value));
}

export function formatDate(value) {
  if (!value) {
    return '-';
  }
  const parsed = new Date(value);
  if (Number.isNaN(parsed.getTime())) {
    return '-';
  }
  return new Intl.DateTimeFormat(LOCALE, {
    month: 'short',
    day: '2-digit',
    year: 'numeric',
  }).format(parsed);
}

export function formatScore(value) {
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '-';
  }
  return Number(value).toFixed(2);
}

export function toCsv(rows) {
  if (!Array.isArray(rows) || rows.length === 0) {
    return '';
  }
  const header = Object.keys(rows[0]).join(',');
  const body = rows
    .map((row) => Object.values(row).map((cell) => `"${String(cell)}"`).join(','))
    .join('\n');
  return `${header}\n${body}`;
}
