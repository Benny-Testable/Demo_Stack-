import { defineStore } from 'pinia';
import type { RecordRow } from '../composables/useRecords';

export const useRecordStore = defineStore('records', {
  state: () => ({ items: [] as RecordRow[], loading: false }),
  getters: {
    open: (state) => state.items.filter((r) => r.status === 'open'),
    totalCents: (state) => state.items.reduce((sum, r) => sum + r.amountCents, 0),
  },
  actions: {
    set(items: RecordRow[]): void {
      this.items = items;
    },
    close(id: number): void {
      const row = this.items.find((r) => r.id === id);
      if (row) {
        row.status = 'closed';
      }
    },
  },
});
