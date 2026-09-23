<?php

declare(strict_types=1);

namespace App\Services;

use App\Models\Record;

/**
 * NOTE: intentionally near-identical to its sibling builder so duplication
 * detectors report a real clone pair.
 */
class InvoiceBuilder
{
    /** @param iterable<Record> $records */
    public function build(iterable $records, string $tenantSlug): array
    {
        $rows = [];
        $total = 0;
        $open = 0;

        foreach ($records as $record) {
            $amount = $record->amount_cents;
            $total += $amount;

            if ($record->status === 'open') {
                $open++;
            }

            $rows[] = [
                'id' => $record->id,
                'title' => $record->title,
                'category' => $record->category,
                'amount' => round($amount / 100, 2),
                'status' => $record->status,
            ];
        }

        usort($rows, static fn (array $a, array $b): int => $b['amount'] <=> $a['amount']);

        return [
            'type' => 'invoice',
            'title' => 'Invoice for ' . $tenantSlug,
            'generated_at' => gmdate('c'),
            'tenant' => $tenantSlug,
            'row_count' => count($rows),
            'open_count' => $open,
            'total_amount' => round($total / 100, 2),
            'rows' => $rows,
        ];
    }
}
