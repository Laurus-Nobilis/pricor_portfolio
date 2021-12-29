<?php
namespace App\Model\Entity;

use Cake\ORM\Entity;

/**
 * Userunit Entity
 *
 * @property int $id
 * @property int $user_id
 * @property int $unit_id
 * @property int $exp
 * @property int $level
 * @property int $equipment1
 * @property int $equipment2
 * @property int $equipment3
 * @property int $equipment4
 *
 * @property \App\Model\Entity\User $user
 * @property \App\Model\Entity\Unit $unit
 */
class Userunit extends Entity
{

    /**
     * Fields that can be mass assigned using newEntity() or patchEntity().
     *
     * Note that when '*' is set to true, this allows all unspecified fields to
     * be mass assigned. For security purposes, it is advised to set '*' to false
     * (or remove it), and explicitly make individual fields accessible as needed.
     *
     * @var array
     */
    protected $_accessible = [
        'user_id' => true,
        'unit_id' => true,
        'exp' => true,
        'level' => true,
        'equipment1' => true,
        'equipment2' => true,
        'equipment3' => true,
        'equipment4' => true,
        'user' => true,
        'unit' => true
    ];
}
