import { describe, it, expect, vi, afterEach } from 'vitest';
import { fetchProperties, fetchLeases } from '../api/client.js';

afterEach(() => {
  vi.restoreAllMocks();
});

describe('api client', () => {
  it('returns parsed properties', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({
      ok: true,
      json: () => Promise.resolve([{ id: 1, name: 'Harbour Block' }]),
    }));

    await expect(fetchProperties()).resolves.toHaveLength(1);
  });

  it('throws when the response is not ok', async () => {
    vi.stubGlobal('fetch', vi.fn().mockResolvedValue({ ok: false, status: 500 }));

    await expect(fetchLeases()).rejects.toThrow('failed with 500');
  });
});
