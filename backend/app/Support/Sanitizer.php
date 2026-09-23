<?php

declare(strict_types=1);

namespace App\Support;

class Sanitizer
{
    public function text(string $value): string
    {
        return trim(strip_tags($value));
    }

    public function html(string $value): string
    {
        return htmlspecialchars($value, ENT_QUOTES | ENT_SUBSTITUTE, 'UTF-8');
    }

    /** Escapes LIKE wildcards so a search term cannot widen its own match. */
    public function searchTerm(string $value): string
    {
        $value = trim($value);

        return str_replace(['%', '_', '\'], ['\%', '\_', '\\'], $value);
    }

    /** Blocks directory traversal in tenant-supplied file names. */
    public function fileName(string $value): string
    {
        return preg_replace('/[^A-Za-z0-9._-]/', '', str_replace('..', '', $value)) ?? '';
    }
}
