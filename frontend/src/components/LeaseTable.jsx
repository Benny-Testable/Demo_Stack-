import React from 'react';
import { formatCurrency, formatDate } from '../utils/formatters.js';

export default function LeaseTable({ leases, showTotals }) {
  const columnCount = 6;

  if (!leases.length) {
    return <p>No leases loaded.</p>;
  }

  return (
    <section>
      <h2>Leases</h2>
      <table>
        <thead>
          <tr>
            <th>Reference</th><th>Property</th><th>Tenant</th>
            <th>Rent</th><th>Term</th><th>Status</th>
          </tr>
        </thead>
        <tbody>
          {leases.map((lease) => (
            <tr key={lease.id}>
              <td>{lease.reference}</td>
              <td>{lease.property}</td>
              <td>{lease.tenant}</td>
              <td>{formatCurrency(lease.baseAnnualRent)}</td>
              <td>{formatDate(lease.startDate)} &ndash; {formatDate(lease.endDate)}</td>
              <td>{lease.status}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </section>
  );
}
