<?php

declare(strict_types=1);

namespace Tests\Unit;

use App\Support\Sanitizer;
use PHPUnit\Framework\TestCase;

final class SanitizerTest extends TestCase
{
    private Sanitizer $sanitizer;

    protected function setUp(): void
    {
        $this->sanitizer = new Sanitizer();
    }

    public function testTextStripsTagsAndTrims(): void
    {
        self::assertSame('hello', $this->sanitizer->text('  <b>hello</b> '));
    }

    public function testHtmlEscapesQuotesAndBrackets(): void
    {
        self::assertSame('&lt;script&gt;', $this->sanitizer->html('<script>'));
        self::assertStringContainsString('&quot;', $this->sanitizer->html('say "hi"'));
    }

    public function testSearchTermEscapesLikeWildcards(): void
    {
        self::assertSame('100\%', $this->sanitizer->searchTerm('100%'));
        self::assertSame('a\_b', $this->sanitizer->searchTerm('a_b'));
    }

    public function testFileNameBlocksTraversal(): void
    {
        self::assertSame('etcpasswd', $this->sanitizer->fileName('../etc/passwd'));
    }
}
