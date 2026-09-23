import { beforeEach, describe, expect, it } from 'vitest';
import { createPinia, setActivePinia } from 'pinia';
import { useRecordStore } from '../src/stores/records';
import { SAMPLE_RECORDS } from '../src/fixtures/records';

describe('record store', () => {
  beforeEach(() => setActivePinia(createPinia()));

  it('exposes open records and totals', () => {
    const store = useRecordStore();
    store.set(SAMPLE_RECORDS.star_alpha);

    expect(store.items).toHaveLength(12);
    expect(store.open.length).toBe(9);
    expect(store.totalCents).toBe(97500);
  });

  it('closes a record by id', () => {
    const store = useRecordStore();
    store.set(SAMPLE_RECORDS.star_beta);
    const id = store.items[0].id;

    store.close(id);

    expect(store.items.find((r) => r.id === id)?.status).toBe('closed');
  });
});
