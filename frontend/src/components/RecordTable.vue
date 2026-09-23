<script setup lang="ts">
import type { RecordRow } from '../composables/useRecords';
import { formatCurrency, formatDate } from '../utils/format';

defineProps<{ records: RecordRow[] }>();
</script>

<template>
  <table class="w-full border-collapse overflow-hidden rounded-lg bg-white text-sm shadow-sm">
    <thead class="bg-slate-100 text-left text-slate-600">
      <tr>
        <th class="px-3 py-2">Title</th>
        <th class="px-3 py-2">Category</th>
        <th class="px-3 py-2">Amount</th>
        <th class="px-3 py-2">Status</th>
        <th class="px-3 py-2">Created</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="r in records" :key="r.id" class="border-t border-slate-100">
        <td class="px-3 py-2">{{ r.title }}</td>
        <td class="px-3 py-2">{{ r.category }}</td>
        <td class="px-3 py-2">{{ formatCurrency(r.amountCents) }}</td>
        <td class="px-3 py-2">
          <span :class="r.status === 'open' ? 'text-emerald-600' : 'text-slate-400'">{{ r.status }}</span>
        </td>
        <td class="px-3 py-2">{{ formatDate(r.createdAt) }}</td>
      </tr>
      <tr v-if="records.length === 0">
        <td colspan="5" class="px-3 py-6 text-center text-slate-400">No records</td>
      </tr>
    </tbody>
  </table>
</template>
