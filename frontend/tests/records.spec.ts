import { describe, expect, it } from 'vitest';
import { ref } from 'vue';
import { useRecords } from '../src/composables/useRecords';
import { TENANTS, useTenant } from '../src/composables/useTenant';
import { summariseA } from '../src/utils/duplicate-a';
import { summariseB } from '../src/utils/duplicate-b';

describe('useRecords', () => {
  it('loads the current tenant records', () => {
    const tenant = ref(TENANTS[0]);
    const { records, openCount, totalAmount } = useRecords(tenant);

    expect(records.value).toHaveLength(12);
    expect(openCount.value).toBe(9);
    expect(totalAmount.value).toBeGreaterThan(0);
  });

  it('swaps records when the tenant changes', async () => {
    const tenant = ref(TENANTS[0]);
    const { records } = useRecords(tenant);
    const firstTitle = records.value[0].title;

    tenant.value = TENANTS[1];
    await Promise.resolve();

    expect(records.value[0].title).not.toBe(firstTitle);
  });

  it('both clone helpers agree', () => {
    const tenant = ref(TENANTS[2]);
    const { records } = useRecords(tenant);

    expect(summariseA(records.value)).toEqual(summariseB(records.value));
  });
});

describe('useTenant', () => {
  it('switches to a known tenant and ignores unknown ones', () => {
    const { tenant, switchTenant } = useTenant();

    switchTenant('star_beta');
    expect(tenant.value.slug).toBe('star_beta');

    switchTenant('does_not_exist');
    expect(tenant.value.slug).toBe('star_beta');
  });
});
