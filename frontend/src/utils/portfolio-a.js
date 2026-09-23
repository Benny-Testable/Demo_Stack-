import { summariseLeases } from './portfolio-b.js';

export function summariseProperties(properties) {
  return properties.map((property) => ({
    id: property.id,
    name: property.name,
    leaseCount: property.leaseCount ?? 0,
  }));
}

export function portfolioTotals(properties, leases) {
  return {
    properties: summariseProperties(properties).length,
    leases: summariseLeases(leases).length,
  };
}
