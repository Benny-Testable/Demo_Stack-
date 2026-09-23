<script setup lang="ts">
import type { Tenant } from '../composables/useTenant';

defineProps<{ tenants: Tenant[]; current: Tenant }>();
const emit = defineEmits<{ switch: [slug: string] }>();
</script>

<template>
  <select
    class="rounded border border-slate-300 bg-white px-3 py-2 text-sm"
    :value="current.slug"
    @change="emit('switch', ($event.target as HTMLSelectElement).value)"
  >
    <option v-for="t in tenants" :key="t.slug" :value="t.slug">{{ t.name }}</option>
  </select>
</template>
