import { describe, it, expect } from 'vitest';
import { render, screen } from '@testing-library/react';
import ProgrammeList from '../components/ProgrammeList.jsx';

describe('ProgrammeList', () => {
  it('shows a message when empty', () => {
    render(<ProgrammeList programmes={[]} />);
    expect(screen.getByText('No programmes loaded.')).toBeInTheDocument();
  });

  it('renders an item per programme', () => {
    render(<ProgrammeList programmes={[
      { id: 1, code: 'CMG-MERIT', name: 'Community Merit Award', category: 'Merit',
        placesAvailable: 20, awardAmount: 6500 },
      { id: 2, code: 'CMG-ACCESS', name: 'Access and Opportunity Grant', category: 'Need',
        placesAvailable: 35, awardAmount: 9000 },
    ]} />);

    expect(screen.getAllByRole('listitem')).toHaveLength(2);
    expect(screen.getByText(/Community Merit Award/)).toBeInTheDocument();
  });
});
