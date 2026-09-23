import { ref } from 'vue';

export interface Tenant {
  slug: string;
  name: string;
  plan: 'starter' | 'growth' | 'enterprise';
}

/** Mirrors the backend landlord seeder; each tenant has its own database. */
export const TENANTS: Tenant[] = [
  { slug: 'star_alpha', name: 'Star Alpha Ltd', plan: 'enterprise' },
  { slug: 'star_beta', name: 'Star Beta GmbH', plan: 'growth' },
  { slug: 'star_gamma', name: 'Star Gamma SARL', plan: 'starter' },
];

export function useTenant() {
  const tenants = ref<Tenant[]>(TENANTS);
  const tenant = ref<Tenant>(TENANTS[0]);

  function switchTenant(slug: string): void {
    const next = TENANTS.find((t) => t.slug === slug);
    if (next) {
      tenant.value = next;
    }
  }

  return { tenant, tenants, switchTenant };
}
