<?php
namespace App\Test\TestCase\Model\Table;

use App\Model\Table\UserdataTable;
use Cake\ORM\TableRegistry;
use Cake\TestSuite\TestCase;

/**
 * App\Model\Table\UserdataTable Test Case
 */
class UserdataTableTest extends TestCase
{

    /**
     * Test subject
     *
     * @var \App\Model\Table\UserdataTable
     */
    public $Userdata;

    /**
     * Fixtures
     *
     * @var array
     */
    public $fixtures = [
        'app.userdata'
    ];

    /**
     * setUp method
     *
     * @return void
     */
    public function setUp()
    {
        parent::setUp();
        $config = TableRegistry::getTableLocator()->exists('Userdata') ? [] : ['className' => UserdataTable::class];
        $this->Userdata = TableRegistry::getTableLocator()->get('Userdata', $config);
    }

    /**
     * tearDown method
     *
     * @return void
     */
    public function tearDown()
    {
        unset($this->Userdata);

        parent::tearDown();
    }

    /**
     * Test initialize method
     *
     * @return void
     */
    public function testInitialize()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test validationDefault method
     *
     * @return void
     */
    public function testValidationDefault()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }

    /**
     * Test defaultConnectionName method
     *
     * @return void
     */
    public function testDefaultConnectionName()
    {
        $this->markTestIncomplete('Not implemented yet.');
    }
}
