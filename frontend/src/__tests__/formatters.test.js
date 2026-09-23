import { describe, it, expect } from 'vitest';
import { formatCurrency, formatDate, formatPercent, toCsv } from '../utils/formatters.js';

describe('formatCurrency', () => {
  it('formats numbers as GBP', () => {
    expect(formatCurrency(1234.5)).toContain('1,234.50');
  });

  it('returns a dash for invalid input', () => {
    expect(formatCurrency(null)).toBe('-');
    expect(formatCurrency('abc')).toBe('-');
  });
});

describe('formatDate', () => {
  it('formats an ISO date', () => {
    expect(formatDate('2024-03-09T00:00:00Z')).toContain('2024');
  });

  it('returns a dash for an unparseable value', () => {
    expect(formatDate('not-a-date')).toBe('-');
    expect(formatDate(null)).toBe('-');
  });
});

describe('formatPercent', () => {
  it('renders one decimal place', () => {
    expect(formatPercent(63.25)).toBe('63.3%');
  });
});

describe('toCsv', () => {
  it('returns an empty string for no rows', () => {
    expect(toCsv([])).toBe('');
  });

  it('builds a header row and body', () => {
    const csv = toCsv([{ a: 1, b: 2 }]);
    expect(csv.split('\n')[0]).toBe('a,b');
  });
});
