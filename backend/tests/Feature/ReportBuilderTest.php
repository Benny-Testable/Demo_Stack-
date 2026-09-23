<?php

declare(strict_types=1);

namespace Tests\Feature;

use App\Models\Record;
use App\Services\InvoiceBuilder;
use App\Services\ReportBuilder;
use PHPUnit\Framework\TestCase;

final class ReportBuilderTest extends TestCase
{
    /** @return array<int, Record> */
    private function records(): array
    {
        return [
            new Record(['title' => 'A', 'category' => 'billing', 'amount_cents' => 5000, 'status' => 'open']),
            new Record(['title' => 'B', 'category' => 'support', 'amount_cents' => 12500, 'status' => 'closed']),
        ];
    }

    public function testReportTotalsAndOrdering(): void
    {
        $out = (new ReportBuilder())->build($this->records(), 'star_alpha');

        self::assertSame('report', $out['type']);
        self::assertSame(175.0, $out['total_amount']);
        self::assertSame(1, $out['open_count']);
        self::assertSame('B', $out['rows'][0]['title']);
    }

    public function testInvoiceSharesTheSameShape(): void
    {
        $out = (new InvoiceBuilder())->build($this->records(), 'star_beta');

        self::assertSame('invoice', $out['type']);
        self::assertSame(2, $out['row_count']);
        self::assertSame('Invoice for star_beta', $out['title']);
    }
}
