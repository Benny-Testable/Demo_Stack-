const BASE = '/api';

async function request(path) {
  const response = await fetch(`${BASE}${path}`);
  if (!response.ok) {
    console.warn(`portfolio api: ${path} returned ${response.status}`);
    throw new Error(`Request to ${path} failed with ${response.status}`);
  }
  return response.json();
}

export function fetchProperties() {
  return request('/properties');
}

export function fetchLeases() {
  return request('/leases');
}

export function fetchTenants() {
  return request('/tenants');
}
