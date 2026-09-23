<script setup lang="ts">
import { computed, ref } from 'vue';
import TenantSwitcher from './components/TenantSwitcher.vue';
import RecordTable from './components/RecordTable.vue';
import MetricCard from './components/MetricCard.vue';
import { useRecords } from './composables/useRecords';
import { useTenant } from './composables/useTenant';

const { tenant, tenants, switchTenant } = useTenant();
const { records, totalAmount, openCount } = useRecords(tenant);
const query = ref('');

const visible = computed(() =>
  records.value.filter((r) => r.title.toLowerCase().includes(query.value.toLowerCase())),
);
</script>

<template>
  <main class="mx-auto max-w-5xl p-6">
    <header class="mb-6 flex items-center justify-between">
      <h1 class="text-2xl font-semibold text-star-700">Star console</h1>
      <TenantSwitcher :tenants="tenants" :current="tenant" @switch="switchTenant" />
    </header>

    <section class="mb-6 grid grid-cols-1 gap-4 sm:grid-cols-3">
      <MetricCard label="Records" :value="records.length" />
      <MetricCard label="Open" :value="openCount" />
      <MetricCard label="Total" :value="totalAmount" prefix="$" />
    </section>

    <input
      v-model="query"
      class="mb-4 w-full rounded border border-slate-300 px-3 py-2"
      placeholder="Filter records"
    />

    <RecordTable :records="visible" />
  </main>
</template>
