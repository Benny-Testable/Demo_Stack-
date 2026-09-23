import React from 'react';
import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import PropertyList from '../components/PropertyList.jsx';

describe('PropertyList', () => {
  it('shows a message when there are no properties', () => {
    render(<PropertyList properties={[]} />);
    expect(screen.getByText('No properties loaded.')).toBeInTheDocument();
  });

  it('renders a row per property', () => {
    render(<PropertyList properties={[
      { id: 1, name: 'Harbour Block', city: 'Bristol', blockNumber: 1, totalUnits: 18, grade: 'Premium' },
      { id: 2, name: 'Foundry Block', city: 'Sheffield', blockNumber: 3, totalUnits: 26, grade: 'Standard' },
    ]} />);

    expect(screen.getAllByRole('listitem')).toHaveLength(2);
    expect(screen.getByText(/Harbour Block/)).toBeInTheDocument();
  });
});
