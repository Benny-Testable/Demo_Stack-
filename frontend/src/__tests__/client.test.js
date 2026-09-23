import { describe, it, expect, vi, afterEach } from 'vitest';
import { fetchProgrammes, fetchApplications } from '../api/client.js';

afterEach(() => {
  vi.restoreAllMocks();
});

describe('api client', () => {
  it('returns parsed programmes', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve([{ id: 1, code: 'CMG-MERIT' }]),
    }));

    await expect(fetchProgrammes()).resolves.toHaveLength(1);
  });

  it('throws when the response is not ok', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({ ok: false, status: 404 }));

    await expect(fetchApplications()).rejects.toThrow('failed with 404');
  });
});
