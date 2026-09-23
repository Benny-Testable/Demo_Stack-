import type { MembershipTier } from '../types';

const LOCALE = 'en-US';

export function formatDate(value: string | null | undefined): string {
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

export function formatDateTime(value: string | null | undefined): string {
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
    hour: '2-digit',
    minute: '2-digit',
  }).format(parsed);
}

export function tierLabel(tier: MembershipTier): string {
  switch (tier) {
    case 'Elite':
      return 'Elite';
    case 'Premium':
      return 'Premium';
    case 'Standard':
      return 'Standard';
    default:
      return 'Flex';
  }
}

export function toDelimited(rows: Record<string, unknown>[]): string {
  if (!Array.isArray(rows) || rows.length === 0) {
    return '';
  }
  const header = Object.keys(rows[0]).join(',');
  const body = rows
    .map((row) => Object.values(row).map((cell) => `"${String(cell)}"`).join(','))
    .join('\n');
  return `${header}\n${body}`;
}
