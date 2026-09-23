import { summariseProperties } from './portfolio-a.js';

export function summariseLeases(leases) {
  return leases.map((lease) => ({
    id: lease.id,
    reference: lease.reference,
    rent: lease.baseAnnualRent ?? 0,
  }));
}

export function leaseCoverage(properties, leases) {
  const propertyCount = summariseProperties(properties).length;
  return propertyCount === 0 ? 0 : leases.length / propertyCount;
}
