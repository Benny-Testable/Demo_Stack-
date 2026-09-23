import React from 'react';
import { formatCurrency } from '../utils/formatters.js';

export default function PropertyList({ properties }) {
  if (!properties.length) {
    return <p>No properties loaded.</p>;
  }

  return (
    <section>
      <h2>Properties</h2>
      <ul>
        {properties.map((property) => (
          <li key={property.id}>
            <strong>Block {property.blockNumber}</strong> {property.name} &mdash; {property.city}
            {' '}({property.totalUnits} units, {property.grade})
            {property.annualRent ? ` ${formatCurrency(property.annualRent)}` : ''}
          </li>
        ))}
      </ul>
    </section>
  );
}
