<?php

declare(strict_types=1);

use App\Http\Controllers\HealthController;
use App\Http\Controllers\RecordController;
use App\Http\Middleware\ResolveTenant;
use Illuminate\Support\Facades\Route;

Route::get('/health', [HealthController::class, 'health']);
Route::get('/ready', [HealthController::class, 'ready']);

Route::middleware(ResolveTenant::class)->prefix('api')->group(function (): void {
    Route::get('/records', [RecordController::class, 'index']);
    Route::post('/records', [RecordController::class, 'store']);
    Route::get('/records/search', [RecordController::class, 'search']);
    Route::post('/records/{id}/close', [RecordController::class, 'close']);
});
