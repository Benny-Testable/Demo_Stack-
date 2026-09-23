import { computed, ref, watch, type Ref } from 'vue';
import type { Tenant } from './useTenant';
import { SAMPLE_RECORDS } from '../fixtures/records';

export interface RecordRow {
  id: number;
  title: string;
  category: string;
  amountCents: number;
  status: 'open' | 'closed';
  createdAt: string;
}

export function useRecords(tenant: Ref<Tenant>) {
  const records = ref<RecordRow[]>(SAMPLE_RECORDS[tenant.value.slug] ?? []);

  watch(tenant, (next) => {
    records.value = SAMPLE_RECORDS[next.slug] ?? [];
  });

  const openCount = computed(() => records.value.filter((r) => r.status === 'open').length);

  const totalAmount = computed(() =>
    Number((records.value.reduce((sum, r) => sum + r.amountCents, 0) / 100).toFixed(2)),
  );

  return { records, openCount, totalAmount };
}
