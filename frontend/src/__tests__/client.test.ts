import { describe, it, expect, vi, afterEach } from 'vitest';
import { fetchMembers, fetchClasses } from '../api/client';

afterEach(() => {
  vi.restoreAllMocks();
});

describe('api client', () => {
  it('returns parsed members', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve([{ id: 'a', fullName: 'Dana Reyes' }]),
    }));

    await expect(fetchMembers()).resolves.toHaveLength(1);
  });

  it('throws when the response is not ok', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({ ok: false, status: 503 }));

    await expect(fetchClasses()).rejects.toThrow('failed with 503');
  });
});
