import { describe, it, expect } from 'vitest';
import { formatDate, formatDateTime, tierLabel, toCsv } from '../utils/format';

describe('formatDate', () => {
  it('formats an ISO date', () => {
    expect(formatDate('2026-09-01T00:00:00Z')).toContain('2026');
  });

  it('returns a dash for missing or invalid input', () => {
    expect(formatDate(null)).toBe('-');
    expect(formatDate('nope')).toBe('-');
  });
});

describe('formatDateTime', () => {
  it('includes a time component', () => {
    expect(formatDateTime('2026-09-01T07:30:00Z')).toMatch(/\d{2}:\d{2}/);
  });

  it('returns a dash for invalid input', () => {
    expect(formatDateTime(undefined)).toBe('-');
  });
});

describe('tierLabel', () => {
  it('maps every tier', () => {
    expect(tierLabel('Elite')).toBe('Elite');
    expect(tierLabel('Premium')).toBe('Premium');
    expect(tierLabel('Standard')).toBe('Standard');
    expect(tierLabel('Flex')).toBe('Flex');
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
