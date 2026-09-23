<?php

declare(strict_types=1);

namespace App\Services;

use App\Models\Record;
use App\Support\Sanitizer;

class RecordService
{
    public function __construct(private readonly Sanitizer $sanitizer)
    {
    }

    /**
     * @param array<string, mixed> $input
     */
    public function create(array $input): Record
    {
        $record = new Record();
        $record->title = $this->sanitizer->text((string) ($input['title'] ?? ''));
        $record->body = $this->sanitizer->html((string) ($input['body'] ?? ''));
        $record->category = (string) ($input['category'] ?? 'general');
        $record->amount_cents = (int) ($input['amount_cents'] ?? 0);
        $record->status = 'open';
        $record->save();

        return $record;
    }

    /**
     * @return array<int, array<string, mixed>>
     */
    public function search(string $query, int $limit = 25): array
    {
        $needle = $this->sanitizer->searchTerm($query);

        if ($needle === '') {
            return [];
        }

        return Record::query()
            ->where('title', 'like', "%{$needle}%")
            ->orWhere('body', 'like', "%{$needle}%")
            ->limit($limit)
            ->get()
            ->map(fn (Record $r): array => [
                'id' => $r->id,
                'title' => $r->title,
                'category' => $r->category,
                'amount' => $r->amount(),
            ])
            ->all();
    }

    public function close(Record $record, string $reason): Record
    {
        $record->status = 'closed';
        $record->body .= "\n\nClosed: " . $this->sanitizer->text($reason);
        $record->save();

        return $record;
    }
}
