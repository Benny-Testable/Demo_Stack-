export function isTenantSlug(value: string): boolean {
  return /^[a-z][a-z0-9_]{2,40}$/.test(value);
}

export function isPositiveAmount(cents: unknown): cents is number {
  return typeof cents === 'number' && Number.isFinite(cents) && cents >= 0;
}

export function requireFields<T extends Record<string, unknown>>(input: T, fields: string[]): string[] {
  return fields.filter((f) => input[f] === undefined || input[f] === null || input[f] === '');
}
