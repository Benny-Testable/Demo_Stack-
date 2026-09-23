<?php

declare(strict_types=1);

/**
 * PII-shaped FIXTURE DATA for detector tests (presidio / semgrep-pii).
 * Entirely fabricated. Never load in production code paths.
 */
return [
    'customers' => [
        ['name' => 'Alex Doe', 'email' => 'alex.doe@example.com', 'phone' => '+1-202-555-0143'],
        ['name' => 'Priya Kumar', 'email' => 'priya.kumar@example.org', 'phone' => '+44 20 7946 0958'],
    ],
    'log_lines' => [
        'INFO user alex.doe@example.com signed in from 203.0.113.42',
        'WARN card ending 4242 declined for priya.kumar@example.org',
    ],
];
