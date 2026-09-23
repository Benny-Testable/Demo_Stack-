import { describe, it, expect } from 'vitest';
import { formatCurrency, formatDate, formatScore, toCsv } from '../utils/formatters.js';

describe('formatCurrency', () => {
  it('formats numbers as USD', () => {
    expect(formatCurrency(9000)).toContain('9,000.00');
  });

  it('returns a dash for invalid input', () => {
    expect(formatCurrency(null)).toBe('-');
    expect(formatCurrency('abc')).toBe('-');
  });
});

describe('formatDate', () => {
  it('formats an ISO date', () => {
    expect(formatDate('2026-09-01T00:00:00Z')).toContain('2026');
  });

  it('returns a dash for unparseable values', () => {
    expect(formatDate('nope')).toBe('-');
    expect(formatDate(null)).toBe('-');
  });
});

describe('formatScore', () => {
  it('renders two decimal places', () => {
    expect(formatScore(84.5)).toBe('84.50');
  });

  it('returns a dash for invalid input', () => {
    expect(formatScore(undefined)).toBe('-');
  });
});

describe('toCsv', () => {
  it('returns empty for no rows', () => {
    expect(toCsv([])).toBe('');
  });

  it('writes a header row', () => {
    expect(toCsv([{ a: 1, b: 2 }]).split('\n')[0]).toBe('a,b');
  });
});
