import { describe, expect, it } from 'vitest';
import { mount } from '@vue/test-utils';
import MetricCard from '../src/components/MetricCard.vue';
import RecordTable from '../src/components/RecordTable.vue';
import { SAMPLE_RECORDS } from '../src/fixtures/records';

describe('MetricCard', () => {
  it('renders label and prefixed value', () => {
    const wrapper = mount(MetricCard, { props: { label: 'Total', value: 42, prefix: '$' } });
    expect(wrapper.text()).toContain('Total');
    expect(wrapper.text()).toContain('$42');
  });
});

describe('RecordTable', () => {
  it('renders a row per record', () => {
    const wrapper = mount(RecordTable, { props: { records: SAMPLE_RECORDS.star_alpha } });
    expect(wrapper.findAll('tbody tr')).toHaveLength(12);
  });

  it('shows an empty state', () => {
    const wrapper = mount(RecordTable, { props: { records: [] } });
    expect(wrapper.text()).toContain('No records');
  });
});
