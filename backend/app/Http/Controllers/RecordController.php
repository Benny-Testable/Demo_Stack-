<?php

declare(strict_types=1);

namespace App\Http\Controllers;

use App\Models\Record;
use App\Services\RecordService;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;

class RecordController
{
    public function __construct(private readonly RecordService $records)
    {
    }

    public function index(Request $request): JsonResponse
    {
        return new JsonResponse([
            'data' => Record::query()->latest()->limit((int) $request->query('limit', '25'))->get(),
        ]);
    }

    public function store(Request $request): JsonResponse
    {
        $record = $this->records->create($request->all());

        return new JsonResponse(['data' => $record], 201);
    }

    public function search(Request $request): JsonResponse
    {
        return new JsonResponse(['data' => $this->records->search((string) $request->query('q', ''))]);
    }

    public function close(Request $request, string $id): JsonResponse
    {
        $record = Record::query()->findOrFail($id);

        return new JsonResponse(['data' => $this->records->close($record, (string) $request->input('reason', ''))]);
    }
}
